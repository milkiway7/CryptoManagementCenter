import React, { useState, useEffect } from 'react'

export const Charts = () => {
    useEffect(() => {
        fetch("api/crypto/linechart", {
            method: 'GET'
        }).then(response => {
            console.log(response)
            return response.json()
        }).then(data => {
            console.log(data)
        }).catch(error => {
            console.log(error);
        })
    },[])
    return (
        <div>
            <h1>React chart</h1>
        </div>
    )
}