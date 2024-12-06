import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Movie, Meta } from '@/types/api'

export const getMovies = (
  query: string | undefined,
  page: number | undefined
): Promise<{
  data: Movie[]
  meta: Meta
}> => {
  return api.get(`/movies/paginated`, {
    params: {
      query,
      page
    }
  })
}

export const getAllMovies = () => {
  const response = api.get(`/movies`) as Promise<Movie[]>
  return response
}

export const getMoviesQueryOptions = ({
  query,
  page
}: { query?: string | undefined; page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['movies', { query, page }] : ['movies'],
    queryFn: () => getMovies(query, page)
  })
}

type UseMoviesOptions = {
  query?: string | undefined
  page?: number
  queryConfig?: QueryConfig<typeof getMoviesQueryOptions>
}

export const useMovies = ({ queryConfig, query, page }: UseMoviesOptions) => {
  return useQuery({
    ...getMoviesQueryOptions({ query, page }),
    ...queryConfig
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

export const getAllMoviesQueryOptions = () => {
  return queryOptions({
    queryKey: ['movies'],
    queryFn: () => getAllMovies()
  })
}
