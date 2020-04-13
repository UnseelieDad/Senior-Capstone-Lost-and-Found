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

    
    lost_items = []
    found_items = []
    matches = []
    with connection.cursor() as cursor:
        cursor.execute("DELETE FROM MatchedItems")
        
        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item from Lost l)")
        for row in cursor:
            lost_item = {
                "id": row[0],
                "color": row[4],
                "type": row[5],
                "location": row[6],
            }
            lost_items.append(lost_item)

        cursor.execute("SELECT * FROM Item i WHERE i.id in (SELECT Item from Found f)")
        for row in cursor:
            found_item = {
                "id": row[0],
                "color": row[4],
                "type": row[5],
                "location": row[6],
            }
            found_items.append(found_item)

        # find potential matches
        for litem in lost_items:
            for fitem in found_items:
                if fitem['color'] == litem['color'] and fitem['type'] == litem['type'] and fitem['location'] == litem['location']:
                    match = {
                        "FoundItem": fitem["id"],
                        "LostItem": litem["id"]
                    }
                    matches.append(match)
                    cursor.execute("Select f.id From Found f Where f.Item = {}".format(match["FoundItem"]))
                    found_item = cursor.fetchone()
                    cursor.execute("Select l.id From Lost l Where l.Item = {}".format(match["LostItem"]))
                    lost_item = cursor.fetchone()
                    cursor.execute("Insert into MatchedItems (FoundItem, LostItem) values (%s, %s)", (found_item, lost_item))

        connection.commit()

    return {
        'statusCode': 200,
        'body': json.dumps(matches)
    }
