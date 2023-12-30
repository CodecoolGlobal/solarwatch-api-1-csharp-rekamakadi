import React, { useState, useEffect } from 'react';
import LoginModal from './LoginModal';
import RequestComponent from './RequestComponent';
import './MainPage.css';

const MainPage = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [apodData, setApodData] = useState(null);
    const [errorFetchingApod, setErrorFetchingApod] = useState(false);


    const handleLogin = () => {
        console.log("Login should be processed...");
        // Implement your authentication logic here
        // Set isAuthenticated to true upon successful login
        setIsAuthenticated(true);
    };

    const fetchApodData = () => {
        const today = new Date().toISOString().slice(0, 10);
        const apiUrl = `https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=${today}`;

        fetch(apiUrl)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch APOD data');
                }
                return response.json();
            })
            .then(data => {
                setApodData(data);
            })
            .catch(error => {
                console.error('Error fetching APOD data:', error);
                setErrorFetchingApod(true);
            });
    };

    useEffect(() => {
        fetchApodData();
    }, []);

    const backgroundImageStyle = apodData
        ? { backgroundImage: `url('${apodData.url}')` }
        : errorFetchingApod
            ? { backgroundImage: `url('https://static3.depositphotos.com/1005809/201/i/450/depositphotos_2011357-stock-photo-sundial.jpg')` }
            : {};

    return (
        <div className="main-container" style={backgroundImageStyle}>
            <h1 className="main-heading">Solar Watch</h1>
            {isAuthenticated ? (
                <RequestComponent />
            ) : (
                <LoginModal onLogin={handleLogin} />
            )}
        </div>
    );
};

export default MainPage;