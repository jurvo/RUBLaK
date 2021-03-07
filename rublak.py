import urllib.request
import urllib

login = '<LOGINID>'
password = '<PASSWORD>'


# Get "public" IP
fp = urllib.request.urlopen("https://login.rz.ruhr-uni-bochum.de/cgi-bin/start")
mystr = fp.read().decode("utf8")
fp.close()
ip = mystr.split("\"")[147]

# Send POST
query_args = { 'code':'1', 'loginid':login, 'password':password, 'ipaddr':ip, 'action':'Login'}
url = 'https://login.rz.ruhr-uni-bochum.de/cgi-bin/laklogin'
data = urllib.parse.urlencode(query_args).encode()
request = urllib.request.Request(url, data)

# for Debuging 
# response = urllib.request.urlopen(request).read()
