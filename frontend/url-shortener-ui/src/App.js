import './App.css';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import LoginSignUpContainer from './components/LoginSignUpContainer/LoginSignUpContainer';
import Dashboard from './components/Dashboard/Dashboard';
import AuthVerify from './services/authenticationService/authenticationService';

function App() {
  return (
    <>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<LoginSignUpContainer/>}/>
            <Route path="/*" element={<Dashboard/>}/>
          </Routes>
          <AuthVerify/>
        </BrowserRouter>
        <ToastContainer />
    </>
  );
}

export default App;
