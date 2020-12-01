import smtplib
import configparser
from email.mime.text import MIMEText

config = configparser.ConfigParser()
config.read('config.cfg')

sender = config.get('From', 'Address')
receivers = [config.get('To', 'Address')]

msg = MIMEText('This is a test message from Mail Server Tester.')

msg['Subject'] = 'Test Subject'
msg['From'] = sender
msg['To'] = receivers[0]

with smtplib.SMTP(config.get('Default', 'Host'), config.get('Default', 'Port')) as server:
    server.login(config.get('Auth', 'Username'), config.get('Auth', 'Password'))
    server.sendmail(sender, receivers, msg.as_string())
    print("Successfully sent email")