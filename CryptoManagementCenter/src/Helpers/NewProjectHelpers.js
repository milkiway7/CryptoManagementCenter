export function processForm(setFormData, updatedStatus, httpRequest) {
    setFormData(prevData => {

        const updatedData = {
            ...prevData,
            status: updatedStatus
        }

        httpRequest(updatedData, setFormData);

        return updatedData;
    })
}

export function addNewProjectPOST(formData, setFormData) {
    fetch("NewProject/AddProjectAsync", {
        method: 'POST',
        headers: {
            'Content-Type':"application/json"
        },
        body: JSON.stringify(formData)
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error, project couldn't be processed / created'. Status: ${response.status}`);
        }
        return response.json();
    }).then(data => {
        if (data.success) {
            setFormData(prevState => ({
                ...prevState,
                id: data.id,
/*                createdAt: data.createdAt*/
            }))
        }
    }).catch(error => {
        console.log(`Error: ${error}`)
    })
}