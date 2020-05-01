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
    new_matches = []
    table_matches = []
    with connection.cursor() as cursor:
        #cursor.execute("DELETE FROM MatchedItems")
        
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
                    
                    cursor.execute("Select f.id From Found f Where f.Item = {}".format(fitem["id"]))
                    found_item = cursor.fetchone()[0]
                    cursor.execute("Select l.id From Lost l Where l.Item = {}".format(litem["id"]))
                    lost_item = cursor.fetchone()[0]

                    match = {
                        "FoundItem": found_item,
                        "LostItem": lost_item
                    }
                    matches.append(match)

        cursor.execute("SELECT mi.FoundItem, mi.LostItem FROM MatchedItems mi")
        for row in cursor:
            table_matches.append((row[0], row[1]))
        
        for match in matches:
            if (match["FoundItem"], match["LostItem"]) in table_matches:
                continue
            else:
                cursor.execute("Insert into MatchedItems (FoundItem, LostItem) values (%s, %s)", (match["FoundItem"], match["LostItem"]))
                new_matches.append(match)
        
        connection.commit()

    return {
        'statusCode': 200,
        'body': json.dumps(new_matches)
    }
