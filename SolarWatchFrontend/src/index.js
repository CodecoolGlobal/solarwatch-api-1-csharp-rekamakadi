import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import MainPage from "./components/MainPage";
import LoginModal from "./components/LoginModal";
import RegistrationPage from "./components/RegistrationPage";
import RequestComponent from "./components/RequestComponent";

const router = createBrowserRouter([
    {
        path: "/",
        element: <MainPage />,
        children: [
            {
                path: "/login",
                element: <LoginModal />,
            },
            {
                path: "/register",
                element: <RegistrationPage />,
            },
            {
                path: "/request",
                element: <RequestComponent />,
            },
        ],
    },
]);

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
    <React.StrictMode>
        <RouterProvider router={router} />
    </React.StrictMode>
);


