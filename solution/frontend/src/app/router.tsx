import { QueryClient, useQueryClient } from '@tanstack/react-query'
import { useMemo } from 'react'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'

import { paths } from '@/config/paths'
import { AppRoot, AppRootErrorBoundary } from './routes/app/root'

export const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: paths.app.root.path,
      element: <AppRoot />,
      ErrorBoundary: AppRootErrorBoundary,
      children: [
        {
          path: paths.app.root.path,
          lazy: async () => {
            const { DashboardRoute } = await import('./routes/app/dashboard')
            return { Component: DashboardRoute }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.movies.path,
          lazy: async () => {
            const { MovieRoute, moviesLoader } = await import('./routes/app/movies/movies')
            return { Component: MovieRoute, loader: moviesLoader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.movie.path,
          lazy: async () => {
            const { MovieRoute, movieLoader } = await import('./routes/app/movies/movie')
            return { Component: MovieRoute, loader: movieLoader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.actors.path,
          lazy: async () => {
            const { ActorsRoute, actorsLoader } = await import('./routes/app/actors/actors')
            return { Component: ActorsRoute, loader: actorsLoader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.actor.path,
          lazy: async () => {
            const { actorLoader, ActorRoute } = await import('./routes/app/actors/actor')
            return { Component: ActorRoute, loader: actorLoader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.directors.path,
          lazy: async () => {
            const { Route, Loader } = await import('./routes/app/directors/directors')
            return { Component: Route, loader: Loader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        },
        {
          path: paths.app.director.path,
          lazy: async () => {
            const { Route, Loader } = await import('./routes/app/directors/director')
            return { Component: Route, loader: Loader(queryClient) }
          },
          ErrorBoundary: AppRootErrorBoundary
        }
      ]
    },
    {
      path: '*',
      lazy: async () => {
        const { NotFoundRoute } = await import('./routes/not-found')
        return {
          Component: NotFoundRoute
        }
      },
      ErrorBoundary: AppRootErrorBoundary
    }
  ])

export const AppRouter = () => {
  const queryClient = useQueryClient()

  const router = useMemo(() => createAppRouter(queryClient), [queryClient])

  return <RouterProvider router={router} />
}
