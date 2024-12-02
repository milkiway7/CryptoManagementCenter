import React, { useState, useEffect } from 'react';

export const NewProject = () => {
    const [test, setTest] = useState({
        firstField: null
    })
    useEffect(() => {
        console.log("a")
    }, [test])
    function handleDataChange(e) {
        const { name, value } = e.target.value
        setTest((prevState) => ({
            ...prevState,
            [name]:value
        }))
    }

    return (
        <div>
            <h1>New project</h1>
            <label>First field</label>
            <input type="text" name="firstField" value={test.firstField} onChange={handleDataChange}></input>
        </div>
    )
}