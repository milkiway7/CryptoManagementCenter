import React from 'react';
import ReactDOM from 'react-dom';
import { NewProject } from './Components/NewProject';
import { NewProjectReport } from './Components/NewProjectReport'
import { BrowserRouter } from 'react-router-dom';

const newProjectView = document.getElementById("newProject");
if (newProjectView) ReactDOM.render(<BrowserRouter><NewProject /></BrowserRouter>, newProjectView);

const newProjectReportView = document.getElementById("new-project-report");
if (newProjectReportView) ReactDOM.render(<BrowserRouter><NewProjectReport /></BrowserRouter>, newProjectReportView)