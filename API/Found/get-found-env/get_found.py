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
    records = []
    with connection.cursor() as cursor:
        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item from Found f)")
        connection.commit()
        for row in cursor:
            record = {
                "id": row[0],
                "email": row[1],
                "description": row[2],
                "create_date": str(row[3]),
                "color": row[4],
                "type": row[5],
                "location": row[6],
                "date_lost": str(row[7]),
                "status": row[8],
                "first_name": row[9],
                "last_name": row[10]
            }
            records.append(record)

    return records