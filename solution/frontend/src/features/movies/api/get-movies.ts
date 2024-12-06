import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Movie, Meta } from '@/types/api'

export const getMovies = (
  page = 1
): Promise<{
  data: Movie[]
  meta: Meta
}> => {
  return api.get(`/movies/paginated`, {
    params: {
      page
    }
  })
}

export const getAllMovies = () => {
  const response = api.get(`/movies`) as Promise<Movie[]>
  return response
}

export const getMoviesQueryOptions = ({ page }: { page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['movies', { page }] : ['movies'],
    queryFn: () => getMovies(page)
  })
}

type UseMoviesOptions = {
  page?: number
  queryConfig?: QueryConfig<typeof getMoviesQueryOptions>
}

export const useMovies = ({ queryConfig, page }: UseMoviesOptions) => {
  return useQuery({
    ...getMoviesQueryOptions({ page }),
    ...queryConfig
  })
}

export const getAllMoviesQueryOptions = () => {
  return queryOptions({
    queryKey: ['movies'],
    queryFn: () => getAllMovies()
  })
}

type UseAllMoviesOptions = {
  queryConfig?: QueryConfig<typeof getAllMoviesQueryOptions>
}

export const useAllMovies = ({ queryConfig }: UseAllMoviesOptions) => {
  return useQuery({
    ...getAllMoviesQueryOptions(),
    ...queryConfig
  })
}
