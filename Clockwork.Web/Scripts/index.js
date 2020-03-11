function UserAction() {
    const selectedTimezone = document.getElementById("selectTimezone").value;
    fetch('http://127.0.0.1:5000/api/currenttime?timezone=' + selectedTimezone)
        .then(
            function (response) {
                if (response.status != 200) {
                    console.log('Something went wrong. Status code: ' + response.status);
                    return;
                }
                response.json().then(function (myObj) {
                    console.log(myObj);
                    // new entry object 
                    const newTimeZoneInfo = myObj.newEntry;
                    // all entries from CurrentTime API / Controller
                    const entries = myObj.allEntries;
                    // add new entry to top of list
                    entries.reverse();
                    // timezone info for new entry
                    const timeZone = newTimeZoneInfo.TimeZone;
                    // timezone and time display 
                    const currTime = moment(newTimeZoneInfo.Time).format('LTS');
                    document.getElementById('outputDiv').innerHTML = 'The time in ' + timeZone + ' is: ' + currTime;
                    // add new entry to list
                    UpdateDatabaseEntriesUI(entries);
                });
            })
        .catch(function (err) {
            console.log("Fetch error", err);
        });
}

function AllEntries() {
    fetch('http://127.0.0.1:5000/api/returneddata')
        .then(
            function (response) {
                if (response.status != 200) {
                    console.log('Something went wrong. Status code: ' + response.status);
                    return;
                }
                response.json().then(function (myData) {
                    console.log(myData);
                    // display array in reverse order
                    myData.reverse();
                    // load/display previous data entries
                    UpdateDatabaseEntriesUI(myData);
                });
            })
        .catch(function (err) {
            console.log("Fetch error", err);
        });
}

function UpdateDatabaseEntriesUI(entries) {
    document.getElementById('dbInfo').innerHTML = "";
    var i;
    var newHtml = "<ul>";
    // create ul for each new timezone entry
    // use li for object properties of timezone
    for (i = 0; i < entries.length; i++) {
        var entry = entries[i];
        const currTime = moment(entry.Time).format('LTS');
        const currUTCTime = moment(entry.UTCTime).format('LTS');
        newHtml += `<li><strong>Timezone: ${entry.TimeZone}</strong></li>
<li>Time: ${currTime}</li>
<li>UTCTime: ${currUTCTime}</li>
<li>ClientIP: ${entry.ClientIp}</li>
<li>TimeQueryID: ${entry.CurrentTimeQueryId}</li><br>`;
    }
    newHtml += "</ul>";
    document.getElementById('dbInfo').innerHTML = newHtml;
}