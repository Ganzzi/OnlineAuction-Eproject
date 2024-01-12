// auth.service.ts
import axios from "axios";

const axiosService = axios.create({
  baseURL: "your_auth_service_base_url",
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor to add the access token to the request headers
axiosService.interceptors.request.use(
  (config) => {
    const accessToken: string = JSON.parse(localStorage.getItem("ACCESS_TOKEN") ?? "");
    if (accessToken !== "") {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor to handle token expiration and refresh
axiosService.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Assuming you have a function to refresh the access token
        const newAccessToken = await refreshAccessToken();

        // Update the original request with the new access token
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        return axios(originalRequest);
      } catch (refreshError) {
        // Handle refresh error, e.g., redirect to login page
        console.error("Failed to refresh access token:", refreshError);
        // Redirect to the login page or handle the error appropriately
      }
    }

    return Promise.reject(error);
  }
);

const refreshAccessToken = async () => {
  // Assuming you have a function to request a new access token using the refresh token
  // This function should make a request to your authentication service
  // and return the new access token
  const refreshToken = JSON.parse(localStorage.getItem("REFRESH_TOKEN") ?? "");
  const response = await axiosService.post("/refresh-token", { refreshToken });
  const newAccessToken = response.data.ACCESS_TOKEN;

  // Update the local storage with the new access token
  localStorage.setItem("ACCESS_TOKEN", JSON.stringify(newAccessToken));

  return newAccessToken;
};

export default axiosService;
