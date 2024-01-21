import { useState } from "react";
import {FaBars, FaRegChartBar, FaTh, FaUserAlt} from "react-icons/fa"
import {NavLink} from "react-router-dom";

const Sidebar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);

    const menuItem = [
        {
            path:"/home",
            name:"Home",
            icon:<FaTh/>
        },
        {
            path:"/grid",
            name:"Grid",
            icon:<FaRegChartBar/>
        },
        {
            path:"/about",
            name:"About",
            icon:<FaUserAlt/>
        }
    ]
    return(
        <div className="container">
            <div style={{width: isOpen ? "250px" : "50px"}} className="sidebar" >
                <div className="top_section">
                    <h1 style={{display: isOpen ? "block" : "none"}} className="logo">Short Linker</h1>
                    <div style={{marginLeft: isOpen ? "50px" : "0px"}} className="bars">
                        <FaBars onClick={toggle}/>
                    </div>
                </div>
                {
                    menuItem.map((item, index)=>(
                        <NavLink to={item.path} key={index} className="link" activeclassname="active">
                            <div className="icon">{item.icon}</div>
                            <div style={{display: isOpen ? "block" : "none"}} className="link_text">{item.name}</div>
                        </NavLink>
                    ))
                }
            </div>
        </div>
    )
}

export default Sidebar;