import logging
import rds_config
import pymysql
import json
import sys

#rds settings

rds_host = "capstonedb.ceg78zil1yli.us-east-2.rds.amazonaws.com"
name = rds_config.db_username
password = rds_config.db_password
db_name = rds_config.db_name

# logging
logger = logging.getLogger()
logger.setLevel(logging.INFO)

# connect using creds from config
try:
    connection = pymysql.connect(rds_host, user=name, passwd=password, db=db_name, connect_timeout=5)
except:
    logger.error("ERROR: Unexpected error: Could not connect to MYSQL instance")
    sys.exit()

logger.info("SUCCESS: Connection to RDS mysql instance succeeded")

def handler(event, context):
    data = {
        'pin': event['pin']
    }
    with connection.cursor() as cursor:
        sql = "select * from Admin where Pin=%s"
        cursor.execute(sql, (event['pin'])) 
        connection.commit()
        #storage for number of matched records and records themselves
        numMatch = 0
        records = []
        #iterate the matched records
        for row in cursor:
            #increase count
            numMatch+=1
            #capture first and last name of admin
            record = {
                "FirstName": row[1],
                "LastName": row[2]
            }
            records.append(record)
        if numMatch == 0:
            #no matched records found, incorrect pin provided
            return {
                'statusCode': 200,
                'body': 'Incorrect PIN provided'
            }
        elif numMatch == 1:
            #exactly one record found, return name of admin
            return {
                'statusCode': 200,
                'body': json.dumps(record)
            }
        else:
            #more than one match found, internal server error -- more than one admin with the pin provided
            return {
                'statusCode': 500,
                'body': 'Internal Server Error'
            }

    
