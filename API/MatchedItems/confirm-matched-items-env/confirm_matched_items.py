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

logger.info("SUCCESS: Connection to RDS mysql instance succeeded.")

# executes upon API event
def handler(event, context):
    # Your code goes here!
    e = event.get("e")
    pi = event.get("pi")
    return e + pi
