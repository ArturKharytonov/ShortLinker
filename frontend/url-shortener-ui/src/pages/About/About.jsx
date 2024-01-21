import React, { useState, useEffect } from 'react';
import axios from 'axios';

const About = () =>{
    const [aboutContent, setAboutContent] = useState('');
    const token = localStorage.getItem('token');

    useEffect(() => {
        const saveChangesButton = document.getElementById('saveChangesButton');

        if (saveChangesButton) {
            saveChangesButton.addEventListener('click', handleSaveChangesClick);
        }

        return () => {
            if (saveChangesButton) {
                saveChangesButton.removeEventListener('click', handleSaveChangesClick);
            }
        };
    }, []);

    useEffect(() => {
        const fetchAboutContent = async () => {
            try {
                const response = await axios.get('https://localhost:7136/about', { headers: { Authorization: `Bearer ${token}`}});
                console.log(response.data);
                setAboutContent(response.data);
            } catch (error) {
                console.error('Error fetching About content:', error.message);
            }
        };

        fetchAboutContent();
    }, []);


    return(
        <div dangerouslySetInnerHTML={{ __html: aboutContent }} />
    )
}
export default About;