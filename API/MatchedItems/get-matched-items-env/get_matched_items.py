import sys
import logging
import pymysql
import json
import boto3

# rds settings
client = boto3.client('ssm', region_name='us-east-2')
rds_host = "capstonedb.ceg78zil1yli.us-east-2.rds.amazonaws.com"
name = client.get_parameter(Name='db_user', WithDecryption=True)['Parameter']['Value']
password = client.get_parameter(Name='db_password', WithDecryption=True)['Parameter']['Value']
db_name = client.get_parameter(Name='db_name', WithDecryption=True)['Parameter']['Value']

# logging
logger = logging.getLogger()
logger.setLevel(logging.INFO)

# connect using creds from rds_config.py
try:
    connection = pymysql.connect(rds_host, user=name, passwd=password, db=db_name, connect_timeout=5)
except:
    logger.error("ERROR: Unexpected error: could not connect ot MySQL instance.")
    sys.exit()

logger.info("SUCCESS: COnnection to RDS mysql instance succeeded.")

# executes upon API event
def handler(event, context):

    found_matches = []
    lost_matches = []

    records = []

    with connection.cursor() as cursor:
        cursor.execute("SELECT * FROM Items i WHERE i.id in (SELECT FoundItem from MatchedItems mi)")
        for row in cursor:
            found_match = {
                "id": row[0],
                "email": row[1],
                "description": row[2],
                "create_date": str(row[3]),
                "color": row[4],
                "type": row[5],
                "location": row[6],
                "date_lost": str(row[7])
            }
            found_matches.append(found_match)

        cursor.execute("SELECT * FROM Items i WHERE i.id (SELECT LostItem from MatchedItems mi)")
        connection.commit()
        for row in cursor:
            lost_match = {
                "id": row[0],
                "email": row[1],
                "description": row[2],
                "create_date": str(row[3]),
                "color": row[4],
                "type": row[5],
                "location": row[6],
                "date_lost": str(row[7])
            }
            lost_matches.append(lost_match)

        for frow, lrow in zip(found_matches, lost_matches):
            record = {
                "found_id": frow[0],
                "found_email": frow[1],
                "found_description": frow[2],
                "found_create_date": str(frow[3]),
                "found_color": frow[4],
                "found_type": frow[5],
                "found_location": frow[6],
                "date_found": str(frow[7]),
                "lost_id": lrow[0],
                "lost_email": lrow[1],
                "lost_description": lrow[2],
                "lost_create_date": str(lrow[3]),
                "lost_color": lrow[4],
                "lost_type": lrow[5],
                "lost_location": lrow[6],
                "date_lost": str(lrow[7])
            }
            records.append(record)

    return records
