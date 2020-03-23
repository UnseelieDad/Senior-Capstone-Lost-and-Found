import sys
import logging
import rds_config
import pymysql
import json

# rds settings
rds_host = "capstonedb.ceg78zil1yli.us-east-2.rds.amazonaws.com"
name = rds_config.db_username
password = rds_config.db_password
db_name = rds_config.db_name

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
        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item from Lost l)")
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
                "first_name": row[8],
                "last_name": row[9]
            }
            records.append(record)

    return records
