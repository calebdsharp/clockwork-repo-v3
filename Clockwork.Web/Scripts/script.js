function ShowTimes() {

    const selectedTimezoneId = document.getElementById('zone').value;

    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            const allTimesObject = JSON.parse(this.response);
            console.log(allTimesObject)

            const currentTimeObject = allTimesObject.CurrentTimeRequestModel;
            const currentTimezone = currentTimeObject.TimeZone;

            const currentTime = moment(currentTimeObject.Time).format('LTS');

            document.getElementById('currentTimeText').innerHTML = 'The time in ' + currentTimezone + ' is: ' + currentTime;

            const requestedTimesObject = allTimesObject.RequestedTimesModel;

            requestedTimesObject.push(currentTimeObject)

            requestedTimesObject.reverse();

            DisplayTimes(requestedTimesObject)

        }
    };
    xhr.open('POST', '/Home/Index?SelectedTimezoneId=' + selectedTimezoneId, true);
    xhr.setRequestHeader('Content-type', 'application/json');
    xhr.send();
}

function AllEntries() {
    fetch('Home/GetRequestedTimes')
        .then(
            function (response) {
                if (response.status != 200) {
                    console.log('Something went wrong. Status code: ' + response.status);
                    return;
                }
                response.json().then(function myList(myData) {

                    myData.reverse();

                    DisplayTimes(myData)

                });
            })
        .catch(function (err) {
            console.log("Fetch error", err);
        });
}


function DisplayTimes(requestedTimesObject) {

    document.getElementById('timezoneDiv').innerHTML = "";

    requestedTimesObject.forEach(timezone => {


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
        timezoneTime.textContent = `Time: ${moment(timezone.Time).format('LTS')}`;

        // Timezone ClientIP
        const timezoneClientIp = document.createElement('li')
        timezoneClientIp.setAttribute('class', 'timezoneCleintIp')
        timezoneClientIp.textContent = `ClientIp: ${timezone.ClientIp}`;

        // Timezone UTCTime
        const timezoneUTCTime = document.createElement('li')
        timezoneUTCTime.setAttribute('class', 'timezoneUTCTime')
        timezoneUTCTime.textContent = `UTCTime: ${moment(timezone.UTCTime).format('LTS')}`;

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

