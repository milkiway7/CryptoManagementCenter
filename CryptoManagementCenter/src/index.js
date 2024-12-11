import React from 'react';
import ReactDOM from 'react-dom';
import { NewProject } from './Components/NewProject';
import { NewProjectReport } from './Components/NewProjectReport'

const newProjectView = document.getElementById("newProject");
if (newProjectView) ReactDOM.render(<NewProject />, newProjectView);

const newProjectReportView = document.getElementById("new-project-report");
if (newProjectReportView) ReactDOM.render(<NewProjectReport/>, newProjectReportView)