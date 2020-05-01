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

# workflow:
# Confirm match button clicked,
# Sends update request for the two ids?
# table doesn't show confirmed matches?
# UPDATE MatchedItems SET Confrimed = 1 WHERE id = ...
# takes the two ids

def handler(event, context):
    data = {
        "update_id": event['match_id']
    }

    update_id = data["update_id"]

    with connection.cursor() as cursor:
        
        # pull lost and found column values from matched items
        cursor.execute(f"SELECT mi.LostItem, mi.FoundItem FROM MatchedItems mi WHERE mi.id = {update_id}")
        ids = cursor.fetchone()
        lost_id = ids[0]
        found_id = ids[1]

        # Remove all other matches with the confirmed items
        cursor.execute(f"DELETE FROM MatchedItems mi WHERE (mi.id != {update_id} AND mi.LostItem = {lost_id}) OR (mi.id != {update_id} AND mi.FoundItem = {found_id})")

        # remove found item from found and item tables
        cursor.execute(f"SELECT f.Item FROM Found f WHERE f.id = {found_id}")
        found_item_id = cursor.fetchone()[0]
        cursor.execute(f"DELETE FROM Found f WHERE f.id = {found_id}")
        cursor.execute(f"DELETE FROM Item i WHERE i.id = {found_item_id}")
        
        # pull item ID from lost table
        cursor.execute(f"SELECT l.Item FROM Lost l WHERE l.id = {lost_id}")
        lost_item_id = cursor.fetchone()[0]

        # update confirmed and lost item reference in matched items
        cursor.execute(f"UPDATE MatchedItems SET Confirmed = 1, ItemId = {lost_item_id} WHERE id = {update_id}")

        # remove lost table entry
        cursor.execute(f"DELETE FROM Lost l WHERE l.id = {lost_id}")

        
        
    connection.commit()

    return {
        'statusCode': 200,
        'body': json.dumps(data)
    }
