import React, { useState, useEffect } from 'react'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { chartConstants  } from '../Constants/chartsConstants'
export const Charts = () => {

    const [lineChart, setLineChart] = useState([]);
    const [symbol, setSymbol] = useState(chartConstants.currencySymbol[0]);
    const [timeRange, setTimeRange] = useState(chartConstants.interval[0]);


    //useEffect(() => { console.log(symbol)},[symbol])
    //useEffect(() => { console.log(timeRange) }, [timeRange])


    useEffect(() => {

        fetch("api/crypto/linechart", {
            method: 'GET'
        }).then(response => {
            if (!response.ok) {
                throw new Error('Cant fetch data from binance');
            }
            console.log(response)
            return response.json()
        }).then(data => {
            console.log(data)
        }).catch(error => {
            console.log(error);
        })

    }, []);

    const data = [
        { time: '2025-01-01 00:00', price: 100 },
        { time: '2025-01-01 01:00', price: 105 },
        { time: '2025-01-01 02:00', price: 110 },
        { time: '2025-01-01 03:00', price: 95 },
        { time: '2025-01-01 04:00', price: 120 },
        { time: '2025-01-01 05:00', price: 125 },
        { time: '2025-01-01 06:00', price: 130 },
        { time: '2025-01-01 07:00', price: 115 },
    ];

    return (
        <div className="line-chart">
            <div className="row px-5">
                <div className="form-group col-3">
                    <label htmlFor="currency" className="mb-1">Currency</label>
                    <select id="currency" className="form-select" name="currency" value={symbol} onChange={(e) => { setSymbol(e.target.value) } }>
                        {chartConstants.currencySymbol.map(currency => {
                            return (
                                <option value={currency}>{currency}</option>
                            )
                        })}
                    </select>
                </div>
                <div className="form-group col-3">
                    <label htmlFor="timeRange" className="mb-1">Interval</label>
                    <select id="timeRange" className="form-select" name="timeRange" value={timeRange} onChange={(e) => { setTimeRange(e.target.value) }}>
                        {chartConstants.timeRange.map(timeRange => {
                            return (
                                <option value={timeRange}>{timeRange}</option>
                            )
                        })}
                    </select>
                </div>
            </div>
            <LineChart width={1200} height={400} data={data}>
                <CartesianGrid stroke="#f5f5f5" />
                <XAxis dataKey="time" />
                <YAxis dataKey="price" />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="price" stroke="#ff7300" />
            </LineChart>
        </div>
    );
}