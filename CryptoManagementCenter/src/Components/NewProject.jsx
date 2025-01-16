import React, { useState, useEffect } from 'react';
import { newProjectConstants } from '../Constants/newProjectConstants';
import { processForm, addNewProjectPOST, updateNewProjectPATCH, mapCurrencyToSymbol } from "../Helpers/NewProjectHelpers";
import { validateFields, convertKeysToLowerCase } from "../Helpers/GenericHelpers";
import { TabChart } from "./Tabs";

export const NewProject = () => {
    const [formData, setFormData] = useState({
        id: null,
        createdAt: null,
        createdBy: null,
        status: newProjectConstants.statuses.empty,
        projectName: null,
        projectDescription: null,
        cryptocurrency: "",
        startDate: null,
        endDate: null,
        investmentAmount: null,
        investmentFund: null,
        investmentType: "",
        investmentStrategy: null
    })
    const [validation, setValidation] = useState({});
    const [activeTab, setActiveTab] = useState('form');
    const [symbol, setSymbol] = useState("");
    const isReadOnly = formData.status == newProjectConstants.statuses.rejected || formData.status == newProjectConstants.statuses.closed

    useEffect(() => {
        const newProjectElement = document.getElementById('newProject');
        const projectData = newProjectElement?.getAttribute('data-new-project');
        if (projectData != "null") {
            const parsedData = convertKeysToLowerCase(JSON.parse(JSON.parse(projectData)))
            setFormData(parsedData)
        }
    }, [])

    useEffect(() => {
        setSymbol(mapCurrencyToSymbol(formData.cryptocurrency));
        //console.log(symbol)
    }, [formData.cryptocurrency])

    function handleFormData(e) {
        const { name, value } = e.target;

        setFormData((prevState) => ({
            ...prevState,
            [name]: value
        }))
    }
    function handleValidation() {
        const fieldsToValidate = newProjectConstants.requiredFields;
        const validationErrors = validateFields(formData, fieldsToValidate);

        setValidation(validationErrors);

        return validationErrors;
    }
    function handleSubmit(e) {
        e.preventDefault();

        const action = e.nativeEvent.submitter.value;

        let validationError = handleValidation();

        switch (action) {
            case "create-project":
                if (Object.keys(validationError).length === 0) {
                    processForm(setFormData, newProjectConstants.statuses.created, addNewProjectPOST)
                }
                break;
            case "reject-project":
                if (Object.keys(validationError).length === 0) {
                    processForm(setFormData, newProjectConstants.statuses.rejected, updateNewProjectPATCH)
                }
                break;
            case "finalize-project":
                if (Object.keys(validationError).length === 0) {
                    processForm(setFormData, newProjectConstants.statuses.closed, updateNewProjectPATCH)
                }
                break;

        }
    }
    function handleTabChange(tab) {
        setActiveTab(tab)
    }
    return (
        <div>
            {formData.status !== newProjectConstants.statuses.empty && 
                <div className="tabs">
                    <button onClick={() => handleTabChange('form')} className={`button-style ${activeTab === 'form' ? 'active' : ''}`}>
                        Form
                    </button>
                    <button onClick={() => handleTabChange('charts')} className={`button-style ${activeTab === 'charts' ? 'active' : ''}`}>
                        Charts
                    </button>
                </div>    
            }


            {activeTab === 'form' && (
                <form method="post" onSubmit={handleSubmit}>
                    {formData.status != newProjectConstants.statuses.empty && <SystemInformation formData={formData} handleFormData={handleFormData} />}
                    <BasicInformation formData={formData} handleFormData={handleFormData} validation={validation} isReadOnly={isReadOnly} />
                    <InvestmentDetails formData={formData} handleFormData={handleFormData} validation={validation} isReadOnly={isReadOnly} />
                    <InvestmentStrategy formData={formData} handleFormData={handleFormData} validation={validation} isReadOnly={isReadOnly} />
                    <ButtonsSection status={formData.status} validation={validation} />
                </form>
            )}

            {activeTab === 'charts' && (
                <TabChart symbol={ symbol } />
            )}

        </div>

    )
}

const SystemInformation = ({ formData, handleFormData }) => {
    return (
        <div className="section">
            <h3>System information</h3>
            <div className="row mb-2">
                <div className="form-group col-3">
                    <label htmlFor="id" className="mb-1">ID</label>
                    <input id="id" type="number" className="form-control" name="id" value={formData.id} onChange={handleFormData} readOnly></input>
                </div>
                <div className="form-group col-3">
                    <label htmlFor="createdAt" className="mb-1">Created at</label>
                    <input id="createdAt" type="datetime-local" className="form-control" name="createdAt" value={formData.createdAt} onChange={handleFormData} readOnly></input>
                </div>
                <div className="form-group col-3">
                    <label htmlFor="createdBy" className="mb-1">Created by</label>
                    <input id="createdBy" type="text" className="form-control" name="createdBy" value={formData.createdBy} onChange={handleFormData} readOnly></input>
                </div>
                <div className="form-group col-3">
                    <label htmlFor="status" className="mb-1">Status </label>
                    <input id="status" type="text" className="form-control" name="status" value={formData.status} onChange={handleFormData} readOnly></input>
                </div>
            </div>
        </div>
    )
}

const BasicInformation = ({ formData, handleFormData, validation, isReadOnly }) => {

    return (
        <div className="section">
            <h3>Basic information</h3>
            <div className="row mb-2">
                <div className="form-group col-6">
                    <label htmlFor="projectName" className="mb-1">Project name </label><span style={{ color: 'red' }}>*</span>
                    <input id="projectName" type="text" className={`form-control ${validation.projectName ? 'is-invalid' : ''}`} name="projectName" value={formData.projectName} onChange={handleFormData} disabled={ isReadOnly }></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="createdBy" className="mb-1">Cryptocurrency</label><span style={{ color: 'red' }}>*</span>
                    <select htmlFor="cryptocurrency" className={`form-select mb-1 ${validation.cryptocurrency ? 'is-invalid' : ''}`} name="cryptocurrency" value={formData.cryptocurrency} onChange={handleFormData} disabled={isReadOnly}>
                        <option value="" disabled>Choose...</option>
                        {newProjectConstants.cryptocurrencies.map(option => {
                            return (
                                <option value={option}>{option}</option>
                            )
                        })}
                    </select>
                </div>
            </div>
            <div className="form-group col-6">
                <label htmlFor="projectDescription" className="mb-1">Project description</label>
                <textarea id="projectDescription" type="text" className="form-control" name="projectDescription" value={formData.projectDescription} onChange={handleFormData} disabled={isReadOnly}></textarea>
            </div>
        </div>
    )
}

const InvestmentDetails = ({ formData, handleFormData, validation, isReadOnly }) => {
    return (
        <div className="section">
            <h3>Investment details</h3>
            <div className="row mb-2">
                <div className="form-group col-6">
                    <label htmlFor="startDate" className="mb-1">Start date</label><span style={{ color: 'red' }}>*</span>
                    <input id="startDate" type="datetime-local" className={`form-control ${validation.startDate ? 'is-invalid' : ''}`} name="startDate" value={formData.startDate} onChange={handleFormData} disabled={isReadOnly}></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="endDate" className="mb-1">End date</label><span style={{ color: 'red' }}>*</span>
                    <input id="endDate" type="datetime-local" className={`form-control ${validation.endDate ? 'is-invalid' : ''}`} name="endDate" value={formData.endDate} onChange={handleFormData} disabled={isReadOnly}></input>
                </div>
            </div>
            <div className="row">
                <div className="form-group col-6">
                    <label htmlFor="investmentAmount" className="mb-1">Investment amount</label><span style={{ color: 'red' }}>*</span>
                    <input id="investmentAmount" type="number" className={`form-control ${validation.investmentAmount ? 'is-invalid' : ''}`} name="investmentAmount" value={formData.investmentAmount} onChange={handleFormData} disabled={isReadOnly}></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="investmentFund" className="mb-1">Investment fund</label>
                    <input id="investmentFund" type="number" className="form-control" name="investmentFund" value={formData.investmentFund} onChange={handleFormData} disabled={isReadOnly}></input>
                </div>
            </div>
        </div>
    )
}

const InvestmentStrategy = ({ formData, handleFormData, validation, isReadOnly }) => {
    return (
        <div className="section">
            <h3>Investment strategy</h3>
            <div className="row">
                <div className="form-group col-6">
                    <label htmlFor="investmentType" className="mb-1">Investment type</label><span style={{ color: 'red' }}>*</span>
                    <select id="investmentType" className={`form-select ${validation.investmentType ? 'is-invalid' : ''} `} name="investmentType" value={formData.investmentType} onChange={handleFormData} disabled={isReadOnly}>
                        <option value="" disabled>Choose...</option>
                        {newProjectConstants.investmentType.map(option => {
                            return (
                                <option value={option}>{option}</option>
                            )
                        })}
                    </select>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="investmentStrategy" className="mb-1">Investment strategy</label><span style={{ color: 'red' }}>*</span>
                    <input id="investmentStrategy" type="text" className={`form-control ${validation.investmentStrategy ? 'is-invalid' : ''}`} name="investmentStrategy" value={formData.investmentStrategy} onChange={handleFormData} disabled={isReadOnly}></input>
                </div>
            </div>
        </div>
    )
}

const ButtonsSection = ({ status }) => {
    function renderButtons() {
        switch (status) {
            case newProjectConstants.statuses.empty:
                return (
                    <div className="form-buttons-list">
                        <button className="button-style" type="submit" name="action" value="create-project">
                            Create
                        </button>
                    </div>
                )
                break;
            case newProjectConstants.statuses.created:
                return (
                    <div className="form-buttons-list">
                        <button className="button-style" type="submit" name="action" value="reject-project">
                            Reject
                        </button>
                        <button className="button-style" type="submit" name="action" value="finalize-project">
                            Finalize
                        </button>
                    </div>

                )
        }
    }

    return (
        <div>
            {renderButtons()}
        </div>
    )
}