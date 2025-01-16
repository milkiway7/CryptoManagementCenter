import { dateTimeWithoutSeconds } from "../Helpers/GenericHelpers"
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
                status: data.status,
                createdAt: dateTimeWithoutSeconds(data.createdAt),
                createdBy: data.createdBy
            }))
        }
    }).catch(error => {
        console.log(`Error: ${error}`)
    })
}

export function updateNewProjectPATCH(formData) {
    fetch("NewProject/ProcessForm", {
        method: "PATCH",
        headers: {
            'Content-Type':"application/json"
        },
        body: JSON.stringify(formData)
    }).then(response => {
        if (!response.ok()) {
            throw new Error(`HTTP error, project couldn't be processed / created'. Status: ${response.status}`);
        }
        return response.json()
    }).catch(error => {
        console.log(`Error: ${error}`)
    })
}

export function mapCurrencyToSymbol(currency) {
    switch(currency){
        case "Bitcoin (BTC)":
            return "BTC";
            break;
        case "Ethereum (ETH)":
            return "ETH";
            break;
        case "Binance Coin (BNB)":
            return "BNB";
            break;
        case "Ripple (XRP)":
            return "XRP";
            break;
        case "Cardano (ADA)":
            return "ADA";
            break;
        case "Solana (SOL)":
            return "SOL";
            break;
        case "USD Coin (USDC)":
            return "USDC";
            break;
        case "Dogecoin (DOGE)":
            return "DOGE";
            break;
        case "Polkadot (DOT)":
            return "DOT";
            break;
        case "Litecoin (LTC)":
            return "LTC";
            break;
    }
}