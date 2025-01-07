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