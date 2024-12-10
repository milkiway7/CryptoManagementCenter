export function validateFields(formData, fieldsToValidate) {
    const errors = {};

    fieldsToValidate.forEach(field => {
        if (!formData[field] || formData[field].trim() === '') {
            errors[field]= `${field} is required`
        }
    })

    return errors;
}