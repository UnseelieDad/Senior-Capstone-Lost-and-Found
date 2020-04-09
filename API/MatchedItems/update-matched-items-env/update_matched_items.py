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
        
        cursor.execute("SELECT * FROM Items i WHERE i.id in (SELECT Item from Lost l)")
        for row in cursor:
            lost_item = {
                "id": row[0],
                "email": row[1],
                "description": row[2],
                "create_date": str(row[3]),
                "color": row[4],
                "type": row[5],
                "location": row[6],
                "date_lost": str(row[7])
            }
            lost_items.append(lost_item)

        cursor.execute("SELECT * FROM Items i WHERE i.id (SELECT Item from Found f)")
        for row in cursor:
            found_item = {
                "id": row[0],
                "email": row[1],
                "description": row[2],
                "create_date": str(row[3]),
                "color": row[4],
                "type": row[5],
                "location": row[6],
                "date_lost": str(row[7])
            }
            found_items.append(found_item)

        # find potential matches
        for litem in lost_items:
            for fitem in found_items:
                match_count = 0.0
                for lattr, fattr in zip(litem, fitem):
                    if lattr.value() == fattr.value():
                        match_count += 1
                if match_count/len(litem) >= 0.75:
                    match = {
                        "FoundItem": fitem["id"],
                        "LostItem": litem["id"]
                    }
                    matches.append(match)
                    sql = "Insert into MatchedItem (FoundItem, LostItem) values (%s, %s)"
                    cursor.execute(sql, (match["FoundItem"], match["LostItem"]))


        connection.commit()

    return {
        'statusCode': 200,
        'body': json.dumps(matches)
    }
