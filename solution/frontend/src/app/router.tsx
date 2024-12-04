import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { useMemo } from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

import { paths } from '@/config/paths';
import { AppRoot, AppRootErrorBoundary } from './routes/app/root';

export const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: paths.app.root.path,
      element: (
          <AppRoot />
      ),
      ErrorBoundary: AppRootErrorBoundary,
      children: [
        {
          path: paths.app.root.path,
          lazy: async () => {
            const { DashboardRoute } = await import('./routes/app/dashboard');
            return { Component: DashboardRoute };
          },
          ErrorBoundary: AppRootErrorBoundary,
      },
        {
          path: paths.app.movies.path,
          lazy: async () => {
            const { MovieRoute, moviesLoader } = await import('./routes/app/movies/movies');
            return { Component: MovieRoute, loader: moviesLoader(queryClient) };
          },
          ErrorBoundary: AppRootErrorBoundary,
      },
      {
        path: paths.app.movie.path,
        lazy: async () => {
          const { MovieRoute, movieLoader } = await import('./routes/app/movies/movie');
          return {Component: MovieRoute, loader: movieLoader(queryClient) };
        },
        ErrorBoundary: AppRootErrorBoundary,
      },
      ],
    },
    {
      path: '*',
      lazy: async () => {
        const { NotFoundRoute } = await import('./routes/not-found');
        return {
          Component: NotFoundRoute,
        };
      },
      ErrorBoundary: AppRootErrorBoundary,
    },
  ]);

export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};