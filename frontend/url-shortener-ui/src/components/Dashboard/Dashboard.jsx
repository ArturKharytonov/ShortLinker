import {Navigate, Route, Routes} from "react-router-dom";
import SideBar from '../Sidebar/Sidebar';
import Home from '../../pages/Home/Home';
import Grid from '../../pages/Grid/Grid';
import About from '../../pages/About/About';

import '../../TestCssForApp.css';

const Dashboard = () => {
    return (
        <div style={{display: "flex"}}>
            <SideBar />
            <Routes>
                    <Route path="home" element={<Home/>}/>
                    <Route path="grid" element={<Grid/>}/>
                    <Route
                        index
                        element={<Navigate to="/dashboard/home" replace />}
                    />
                    <Route path="about" element={<About/>}/>
            </Routes>
        </div>
    )
}

export default Dashboard;
