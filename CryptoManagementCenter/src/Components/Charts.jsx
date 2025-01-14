import React, { useState, useEffect } from 'react'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { chartConstants } from '../Constants/chartsConstants'
import { mapTimeRangeToInterval, calculateStartDateForLineChart, formatDateToDateAndTime } from '../Helpers/GenericHelpers'

export const Charts = () => {

    const [lineChart, setLineChart] = useState([]);
    const [symbol, setSymbol] = useState(chartConstants.currencySymbol[0]);
    const [timeRange, setTimeRange] = useState(chartConstants.timeRange[0]);

    useEffect(() => {
        let interval = mapTimeRangeToInterval(timeRange)
        let startTime = calculateStartDateForLineChart(timeRange);

        fetch(`api/crypto/linechart?symbol=${symbol}&interval=${interval}&startTime=${startTime}`, {
            method: 'GET'
        }).then(response => {
            if (!response.ok) {
                throw new Error('Cant fetch data from binance');
            }
            return response.json()
        }).then(data => {
            data.forEach((item) => {
                item.closingTime = formatDateToDateAndTime(item.closingTime);
            })
            setLineChart(data);
        }).catch(error => {
            console.log(error);
        })

    }, [symbol, timeRange]);

    console.log(lineChart[lineChart.length - 1]?.price)


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
            <div className="text-center mt-5">
                <h3>{ symbol }/USD</h3>
            </div>
            <LineChart width={1200} height={400} data={lineChart}>
                <CartesianGrid stroke="#f5f5f5" />
                <XAxis dataKey="closingTime"
                    interval={timeRange === '1D' ? 1 : (timeRange === '7D' ? 12 : (timeRange === '1M' ? 30 : 90))}
                    tickFormatter={(value) => {

                        const date = new Date(value); 
                        if (timeRange === '1D') {
                            return value.slice(11); 
                        } else if (timeRange === '7D') {
                            return String(date.getDate()).padStart(2, '0') + "-" + String(date.getHours()).padStart(2, '0') + ":" + String(date.getMinutes()).padStart(2, '0');
                        } else if (timeRange === '1M') {
                            return String(date.getMonth() + 1).padStart(2, '0') + "-" + String(date.getDate()).padStart(2, '0'); 
                        } else if (timeRange === '3M') {
                            return String(date.getMonth() + 1).padStart(2, '0') + "-" + String(date.getDate()).padStart(2, '0'); 
                        } else if (timeRange === '1Y') {
                            return String(date.getMonth() + 1).padStart(2, '0') + "-" + date.getFullYear(); 
                        }
                    }}
                />
                <YAxis dataKey="price"
                    domain={[
                        (dataMin) => dataMin * 0.95, 
                        (dataMax) => dataMax * 1.05  
                    ]}
                    tickFormatter={(value) => value.toFixed(3)}
                />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="price" stroke="#ff7300" />
            </LineChart>
            <div className="text-center">
                <h3>Current price: ${lineChart[lineChart.length - 1]?.price}</h3>
            </div>
        </div>
    );
}