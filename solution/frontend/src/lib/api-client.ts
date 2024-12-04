import Axios, { InternalAxiosRequestConfig } from 'axios';
import { useNotifications } from '@/components/ui/notifications';
import { paths } from '@/config/paths';

function authRequestInterceptor(config: InternalAxiosRequestConfig) {
  if (config.headers)
    config.headers.Accept = 'application/json';
      
  if (config.method !== 'get') {
    const secretKey = import.meta.env.VITE_SECRET_KEY;
    config.headers.Authorization = `Bearer ${secretKey}`;
  }
  return config;
}

const url = import.meta.env.VITE_BASE_URL
export const api = Axios.create({
  baseURL: url,
});

api.interceptors.request.use(authRequestInterceptor);
api.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    const message = error.response?.data?.message || error.message;
    useNotifications.getState().addNotification({
      type: 'error',
      title: 'Error',
      message,
    });

    if (error.response?.status === 401) {
      //const searchParams = new URLSearchParams();
      //const redirectTo = searchParams.get('redirectTo') || window.location.pathname;
      window.location.href = paths.app.dashboard.getHref();
    }

    return Promise.reject(error);
  },
);
