﻿import React, { useState, useEffect } from 'react';
import { newProjectConstants } from '../Constants/newProjectConstants'
import { processForm, addNewProjectPOST } from "../Helpers/NewProjectHelpers"
import { validateFields } from "../Helpers/GenericHelpers"

export const NewProject = () => {
    const [formData, setFormData] = useState({
        id: 0,
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

        switch (action) {
            case "create-project":
                let validationError = handleValidation();
                console.log(validationError);
                if (Object.keys(validationError).length === 0) {
                    processForm(setFormData, newProjectConstants.statuses.created, addNewProjectPOST)
                }
                break;
        }
    }

    return (
        <form method="post" onSubmit={ handleSubmit }>
            <SystemInformation formData={formData} handleFormData={handleFormData} />
            <BasicInformation formData={formData} handleFormData={handleFormData} validation={validation} />
            <InvestmentDetails formData={formData} handleFormData={handleFormData} validation={validation} />
            <InvestmentStrategy formData={formData} handleFormData={handleFormData} validation={validation} />
            <ButtonsSection status={formData.status} validation={validation} />
        </form>
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

const BasicInformation = ({ formData, handleFormData, validation }) => {

    return (
        <div className="section">
            <h3>Basic information</h3>
            <div className="row mb-2">
                <div className="form-group col-6">
                    <label htmlFor="projectName" className="mb-1">Project name </label><span style={{ color: 'red' }}>*</span>
                    <input id="projectName" type="text" className={`form-control ${validation.projectName ? 'is-invalid' : ''}`} name="projectName" value={formData.projectName} onChange={handleFormData}></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="createdBy" className="mb-1">Cryptocurrency</label><span style={{ color: 'red' }}>*</span>
                    <select htmlFor="cryptocurrency" className={`form-select mb-1 ${validation.cryptocurrency ? 'is-invalid' : ''}`} name="cryptocurrency" value={formData.cryptocurrency} onChange={handleFormData}>
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
                <textarea id="projectDescription" type="text" className="form-control" name="projectDescription" value={formData.projectDescription} onChange={handleFormData}></textarea>
            </div>
        </div>
    )
}

const InvestmentDetails = ({ formData, handleFormData, validation }) => {
    return (
        <div className="section">
            <h3>Investment details</h3>
            <div className="row mb-2">
                <div className="form-group col-6">
                    <label htmlFor="startDate" className="mb-1">Start date</label><span style={{ color: 'red' }}>*</span>
                    <input id="startDate" type="datetime-local" className={`form-control ${validation.startDate ? 'is-invalid' : ''}`} name="startDate" value={formData.startDate} onChange={handleFormData}></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="endDate" className="mb-1">End date</label><span style={{ color: 'red' }}>*</span>
                    <input id="endDate" type="datetime-local" className={`form-control ${validation.endDate ? 'is-invalid' : ''}`} name="endDate" value={formData.endDate} onChange={handleFormData}></input>
                </div>
            </div>
            <div className="row">
                <div className="form-group col-6">
                    <label htmlFor="investmentAmount" className="mb-1">Investment amount</label><span style={{ color: 'red' }}>*</span>
                    <input id="investmentAmount" type="number" className={`form-control ${validation.investmentAmount ? 'is-invalid' : ''}`} name="investmentAmount" value={formData.investmentAmount} onChange={handleFormData}></input>
                </div>
                <div className="form-group col-6">
                    <label htmlFor="investmentFund" className="mb-1">Investment fund</label>
                    <input id="investmentFund" type="number" className="form-control" name="investmentFund" value={formData.investmentFund} onChange={handleFormData}></input>
                </div>
            </div>
        </div>
    )
}

const InvestmentStrategy = ({ formData, handleFormData, validation }) => {
    return (
        <div className="section">
            <h3>Investment strategy</h3>
            <div className="row">
                <div className="form-group col-6">
                    <label htmlFor="investmentType" className="mb-1">Investment type</label><span style={{ color: 'red' }}>*</span>
                    <select id="investmentType" className={`form-select ${validation.investmentType ? 'is-invalid' : ''} `} name="investmentType" value={formData.investmentType} onChange={handleFormData}>
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
                    <input id="investmentStrategy" type="text" className={`form-control ${validation.investmentStrategy ? 'is-invalid' : ''}`} name="investmentStrategy" value={formData.investmentStrategy} onChange={handleFormData}></input>
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
                    <button className="button-style" type="submit" name="action" value="create-project">
                        Create
                    </button>
                )
                break;
        }
    }

    return (
        <div className="form-buttons-list">
            {renderButtons()}
        </div>
    )
}