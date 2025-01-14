export function validateFields(formData, fieldsToValidate) {
    const errors = {};

    fieldsToValidate.forEach(field => {
        if (!formData[field]) {
            errors[field]= `${field} is required`
        }
    })

    return errors;
}

export function dateTimeWithoutSeconds(date) {
    return date.slice(0, 16);
}

export function onlyDate(date) {
    return date.slice(0, 10);
}

export function convertKeysToLowerCase(obj) {
    // Tworzymy nowy obiekt z kluczami w małych literach
    const newObj = {};
    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            // Przypisujemy wartość do nowego obiektu z kluczem w małych literach
            newObj[key.charAt(0).toLowerCase() + key.slice(1)] = obj[key];
        }
    }
    return newObj;
}

export function mapTimeRangeToInterval(timeRange) {
    switch (timeRange) {
        case "1D":
            return "30m";
            break;
        case "7D":
            return "1h";
            break;
        case "1M":
            return "4h";
            break;
        case "3M":
            return "12h";
            break;
        case "1Y":
            return "1d"
            break;
    }
}

export function calculateStartDateForLineChart(timeRange) {
    const currentDate = Date.now();
    const oneDayInMiliseconds = 24 * 60 * 60 * 1000;

    switch (timeRange) {
        case "1D":
            return currentDate - oneDayInMiliseconds;
            break;
        case "7D":
            return currentDate - (7 * oneDayInMiliseconds);
            break;
        case "1M":
            return currentDate - (30 * oneDayInMiliseconds);
            break;
        case "3M":
            return currentDate - (90 * oneDayInMiliseconds);
            break;
        case "1Y":
            return currentDate - (365 * oneDayInMiliseconds);
            break;
    }

}

export function formatDateToDateAndTime(isoString) {
    const date = new Date(isoString);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    const formatedDate = `${year}-${month}-${day} ${hours}:${minutes}`

    return formatedDate;
}