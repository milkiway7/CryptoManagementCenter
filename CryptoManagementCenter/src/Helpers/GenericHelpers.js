export function validateFields(formData, fieldsToValidate) {
    const errors = {};

    fieldsToValidate.forEach(field => {
        if (!formData[field] || formData[field].trim() === '') {
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