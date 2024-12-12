import React, { useState, useEffect } from 'react'
import { onlyDate } from '../Helpers/GenericHelpers'
import { useNavigate } from 'react-router-dom';

export const NewProjectReport = () => {
    const [reportState, setReportState] = useState()

    useEffect(() => {
        fetch('/NewProject/ProjectReport', {
            method: 'GET'
        }).then(response => response.json())
            .then(data => {
                setReportState(data)
            }).catch(error => {
                console.log(`Error: ${error}`)
            })
    }, [])

    return (
        <div className="section">
            <div className="table">
                <table>
                    <thead>
                        <th>ID</th>
                        <th>Created at</th>
                        <th>Created by</th>
                        <th>Status</th>
                        <th>Project name</th>
                        <th>Cryptocurrency</th>
                        <th>Start date</th>
                        <th>End date</th>
                        <th>Investmant amount</th>
                        <th>Investment type</th>
                    </thead>

                    {reportState != null && <TableBody reportState={reportState} />}

                </table>
            </div>
        </div>
    )
}

const TableBody = ({ reportState }) => {

    const navigate = useNavigate();

    function handleRowClick(row) {
        navigate('/NewProject', { state: row });
    };

    return (
        <tbody>
            {reportState.map(row => {
                return (
                    <tr onClick={() => handleRowClick(row)} style={{cursor:'pointer'} }>
                        <td>
                            { row.id }
                        </td>
                        <td>
                            {onlyDate(row.createdAt)}
                        </td>
                        <td>
                            {row.createdBy}
                        </td>
                        <td>
                            {row.status}
                        </td>
                        <td>
                            {row.projectName}
                        </td>
                        <td>
                            {row.cryptocurrency}
                        </td>
                        <td>
                            {row.startDate}
                        </td>
                        <td>
                            {row.endDate}
                        </td>
                        <td>
                            {row.investmentAmount}
                        </td>
                        <td>
                            {row.investmentType}
                        </td>
                    </tr>
                )
            }) }
        </tbody>
    )
}