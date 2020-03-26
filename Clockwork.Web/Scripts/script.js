function ShowCurrentTime() {

    const selectedTimezoneId = document.getElementById('zone').value;

    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            const currentTimeObject = JSON.parse(this.responseText);

            const currentTimezone = currentTimeObject.TimeZone;

            const currentTime = moment(currentTimeObject.Time).format('LTS');

            const timeDiv = document.getElementById('currentTimeDiv');
            timeDiv.style.display = "block";

            document.getElementById('currentTimeText').innerHTML = 'The time in ' + currentTimezone + ' is: ' + currentTime;

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
                response.json().then(function (myData) {

                    myData.reverse();
                    myData.forEach(timezone => {


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

                });
            })
        .catch(function (err) {
            console.log("Fetch error", err);
        });
}

const list = new Array();
const pageList = new Array();
var currentPage = 1;
const numberPerPage = 10;
var numberOfPages = 1;

function getNumberOfPages() {
    return Math.ceil(list.length / numberPerPage);
}

list.push(myData)
console.log(list)
numberOfPages = getNumberOfPages();
console.log(numberOfPages)

function nextPage() {
    currentPage += 1;
    loadList();
}
function previousPage() {
    currentPage -= 1;
    loadList();
}
function firstPage() {
    currentPage = 1;
    loadList();
}
function lastPage() {
    currentPage = numberOfPages;
    loadList();
}
function loadList() {
    var begin = ((currentPage - 1) * numberPerPage);
    var end = begin + numberPerPage;

    pageList = list.slice(begin, end);
    drawList();    // draws out our data
    check();         // determines the states of the pagination buttons
}
function drawList() {
    document.getElementById("list").innerHTML = "";

    for (r = 0; r < pageList.length; r++) {
        document.getElementById("list").innerHTML += pageList[r] + "";
    }
}
function check() {
    document.getElementById("next").disabled = currentPage == numberOfPages ? true : false;
    document.getElementById("previous").disabled = currentPage == 1 ? true : false;
    document.getElementById("first").disabled = currentPage == 1 ? true : false;
    document.getElementById("last").disabled = currentPage == numberOfPages ? true : false;
}

function load() {
    makeList();
    loadList();
}


