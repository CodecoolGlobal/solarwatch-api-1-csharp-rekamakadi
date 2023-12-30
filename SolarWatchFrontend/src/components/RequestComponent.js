import React, { useState } from 'react';
import './RequestComponent.css';

const RequestComponent = () => {
    const [date, setDate] = useState('');
    const [textInput, setTextInput] = useState('');
    const [responseData, setResponseData] = useState(null);

    const handleRequest = async () => {
        try {
            // Send request to the backend with date and textInput
            const response = await fetch.post('/api/request-data', {
                date,
                textInput,
            });

            // Assuming successful request if status is 200
            if (response.status === 200) {
                setResponseData(response.data);
            }
        } catch (error) {
            console.error('Request failed', error);
            // Handle request failure
        }
    };

    return (
        <div className="request-container">
            <h2 className="request-heading">Data Request</h2>
            <label>Date:</label>
            <input
                type="date"
                value={date}
                onChange={(e) => setDate(e.target.value)}
            />
            <label>Text Input:</label>
            <input
                type="text"
                value={textInput}
                onChange={(e) => setTextInput(e.target.value)}
            />
            <button onClick={handleRequest}>Submit Request</button>

            {responseData && (
                <div>
                    <h3>Response:</h3>
                    <pre>{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}
        </div>
    );
};

export default RequestComponent;