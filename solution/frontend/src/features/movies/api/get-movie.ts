import { useQuery, queryOptions } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Movie } from '@/types/api'

export const getMovie = ({
  movieUniqueId
}: {
  movieUniqueId: string
}): Promise<{ data: Movie }> => {
  const response = api.get(`/movies/${movieUniqueId}`)
  return response
}

export const getMovieQueryOptions = (movieUniqueId: string) => {
  return queryOptions({
    queryKey: ['movies', movieUniqueId],
    queryFn: () => getMovie({ movieUniqueId })
  })
}

type UseMovieOptions = {
  movieUniqueId: string
  queryConfig?: QueryConfig<typeof getMovieQueryOptions>
}

export const useMovie = ({ movieUniqueId, queryConfig }: UseMovieOptions) => {
  return useQuery({
    ...getMovieQueryOptions(movieUniqueId),
    ...queryConfig
  })
}
