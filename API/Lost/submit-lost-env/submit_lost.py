import logging
import pymysql
import json
import sys
import boto3

#rds settings

client = boto3.client('ssm', region_name='us-east-2')
rds_host = "capstonedb.ceg78zil1yli.us-east-2.rds.amazonaws.com"
name = client.get_parameter(Name='db_user', WithDecryption=True)['Parameter']['Value']
password = client.get_parameter(Name='db_password', WithDecryption=True)['Parameter']['Value']
db_name = client.get_parameter(Name='db_name', WithDecryption=True)['Parameter']['Value']

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
        'email': event['email'],
        'description': event['description'],
        'color': event['color'],
        'type': event['type'],
        'location': event['location'],
        'date_lost': event['date_lost'],
        'status': event['status'],
        'first_name': event['first_name'],
        'last_name': event['last_name']
    }
    with connection.cursor() as cursor:
        
        sql = "insert into Item (Email, Description, Color, Type, Location, DateLost, Status, FirstName, LastName) values (%s, %s, %s, %s, %s, %s, %s, %s, %s)"
        cursor.execute(sql, (data['email'], data['description'], data['color'], data['type'], data['location'], data['date_lost'], data['status'], data['first_name'], data['last_name'])) 

        cursor.execute("select max(id) from Item")
        
        item_id = cursor.fetchone()[0]

        sql = "insert into Lost (Item) values (%s)"
        cursor.execute(sql, (item_id))
        
        connection.commit()

    return {
        'statusCode': 200,
        'body': json.dumps(data)
    }

    
