import React from 'react';
import ReactDOM from 'react-dom';
import { NewProject } from './Components/NewProject';

const newProjectView = document.getElementById("newProject");
if (newProjectView) ReactDOM.render(<NewProject />, newProjectView);