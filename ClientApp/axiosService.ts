// auth.service.ts
import axios from "axios";

const axiosService = axios.create({
  baseURL: "https://localhost:7073",
  // baseURL: "http://localhost:5106",
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor to add the access token to the request headers
axiosService.interceptors.request.use(
  (config) => {
    if (typeof window !== "undefined") {
      
      const _token: string = localStorage.getItem("ACCESS_TOKEN") ?? "".toString();
      const accessToken: string = JSON.parse(_token);
      
      if (accessToken !== "") {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }
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
      localStorage.removeItem("ACCESS_TOKEN");
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
  console.log('refreshing new token...');
  
  // Assuming you have a function to request a new access token using the old access token
  // This function should make a request to your authentication service
  // and return the new access token
  const _token: string = localStorage.getItem("ACCESS_TOKEN") ?? "".toString();
  const accessToken: string = JSON.parse(_token);

  const response = await axiosService.post("/api/auth/RefreshToken", { accessToken });
  const newAccessToken = response.data?.AccessToken;

  // Update the local storage with the new access token
  localStorage.setItem("ACCESS_TOKEN", JSON.stringify(newAccessToken));

  return newAccessToken;
};

export default axiosService;
