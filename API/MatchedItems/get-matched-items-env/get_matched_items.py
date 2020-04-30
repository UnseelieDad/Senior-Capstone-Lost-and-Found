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
    match_ids = []

    records = []

    with connection.cursor() as cursor:

        cursor.execute("SELECT mi.id FROM MatchedItems mi Where mi.Confirmed = 0")
        for row in cursor:
            match_ids.append(row[0])

        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item FROM Found f WHERE f.id in (SELECT FoundItem from MatchedItems mi Where mi.Confirmed = 0))")
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

        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item FROM Lost l WHERE l.id in (SELECT LostItem from MatchedItems mi Where mi.Confirmed = 0))")
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

        for fmatch, lmatch, mid in zip(found_matches, lost_matches, match_ids):
            record = {
                "match_id": mid,
                "found_id": fmatch["id"],
                "found_email": fmatch["email"],
                "found_description": fmatch["description"],
                "found_create_date": fmatch["create_date"],
                "found_color": fmatch["color"],
                "found_type": fmatch["type"],
                "found_location": fmatch["location"],
                "date_found": fmatch["date_lost"],
                "lost_id": lmatch["id"],
                "lost_email": lmatch["email"],
                "lost_description": lmatch["description"],
                "lost_create_date": lmatch["create_date"],
                "lost_color": lmatch["color"],
                "lost_type": lmatch["type"],
                "lost_location": lmatch["location"],
                "date_lost": lmatch["date_lost"]
            }
            records.append(record)

    return records
