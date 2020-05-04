function AddNewTime() {
    const selectedTimezoneId = document.getElementById('zone').value;
    fetch('/Home/requestedtimezone?selectedTimezoneId=' + selectedTimezoneId, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json'
        }
    }).then(function (response) {
        if (response.status != 200) {
            console.log('Something went wrong. Status code: ' + response.status);
            return;
        }
        response.json().then(function (currentTimeData) {
            const currentTimeDiv = document.getElementById('requestedTimezoneDiv');

            console.log(currentTimeData);

            const entries = currentTimeData.AllTimeQueries.CurrentTimeQueries;
            console.log(entries);

            entries.reverse();

            // timezone info for new entry  
            const timeZone = currentTimeData.CurrentTimeQuery.TimeZone;

            // timezone and time display   
            const currentTime = moment(currentTimeData.CurrentTimeQuery.Time).format('lll');

            currentTimeDiv.textContent = 'The time in ' + timeZone + ' is: ' + currentTime;

            DisplayTimes(entries);
        });
    })
        .catch(function (err) {
            console.log('Fetch error', err);
        });
}

function AllEntries() {
    fetch('Home/GetRequestedTimes')
        .then(
            function (response) {
                if (response.status != 200) {
                    console.log('Something went wrong. Status code: ' + response.status);
                    return;
                }
                response.json().then(function (myData) {
                    console.log(myData);

                    const entries = myData.AllTimeQueries.CurrentTimeQueries;

                    entries.reverse();

                    DisplayTimes(entries);

                });
            })
        .catch(function (err) {
            console.log("Fetch error", err);
        });
}

function DisplayTimes(entries) {

    document.getElementById('timezoneDiv').innerHTML = "";

    entries.forEach(timezone => {

        const timeDiv = document.getElementById('timezoneDiv')

        const timezoneList = document.createElement('ul')
        timezoneList.setAttribute('class', 'timezoneList')

        // Timezone Name   
        const timezoneName = document.createElement('li')
        timezoneName.setAttribute('class', 'timezoneName')
        timezoneName.textContent = `Timezone: ${timezone.TimeZone}`;

        //Timezone Time
        const timezoneTime = document.createElement('li')
        timezoneTime.setAttribute('class', 'timezoneTime')
        timezoneTime.textContent = `Time: ${moment(timezone.Time).format('lll')}`;

        // Timezone ClientIP
        const timezoneClientIp = document.createElement('li')
        timezoneClientIp.setAttribute('class', 'timezoneCleintIp')
        timezoneClientIp.textContent = `ClientIp: ${timezone.ClientIp}`;

        // Timezone UTCTime
        const timezoneUTCTime = document.createElement('li')
        timezoneUTCTime.setAttribute('class', 'timezoneUTCTime')
        timezoneUTCTime.textContent = `UTCTime: ${moment(timezone.UTCTime).format('lll')}`;

        // Timezone CurrentTimeQueryId
        const timezoneCurrentTimeQueryId = document.createElement('li')
        timezoneCurrentTimeQueryId.setAttribute('class', 'timezoneCurrentTimeQueryId')
        timezoneCurrentTimeQueryId.textContent = `CurrentTimeQueryId: ${timezone.CurrentTimeQueryId}`;

        timeDiv.appendChild(timezoneList)

        timezoneList.appendChild(timezoneName)
        timezoneList.appendChild(timezoneTime)
        timezoneList.appendChild(timezoneClientIp)
        timezoneList.appendChild(timezoneUTCTime)
        timezoneList.appendChild(timezoneCurrentTimeQueryId)

    })
}
