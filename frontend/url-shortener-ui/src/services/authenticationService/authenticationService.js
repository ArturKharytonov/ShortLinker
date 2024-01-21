import { useEffect } from "react";

import { useLocation, useNavigate } from "react-router-dom";

const parseJwt = (token) => {
  try {
    return JSON.parse(atob(token.split(".")[1]));
  } catch (e) {
    return null;
  }
};

const AuthVerify = () => {
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const { pathname } = location;

    if (pathname === "/login" || pathname === "/grid") {
      return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    const { exp: tokenExpiration } = parseJwt(token);

    if (tokenExpiration && new Date(tokenExpiration * 1000) < new Date()) {
      localStorage.removeItem("token");
      navigate("/login");
    }
  }, [location, navigate]);

  return null;
};

export default AuthVerify;