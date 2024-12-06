import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Actor, Meta } from '@/types/api'

export const getActors = (
  query: string | undefined,
  page: number | undefined
): Promise<{
  data: Actor[]
  meta: Meta
}> => {
  return api.get(`/actors/paginated`, {
    params: {
      query,
      page
    }
  })
}

export const getAllActors = () => {
  const response = api.get(`/actors`) as Promise<Actor[]>
  return response
}

export const getActorsQueryOptions = ({
  query,
  page
}: { query?: string | undefined; page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['actors', { query, page }] : ['actors'],
    queryFn: () => getActors(query, page)
  })
}

type UseActorsOptions = {
  query?: string | undefined
  page?: number
  queryConfig?: QueryConfig<typeof getActorsQueryOptions>
}

export const useActors = ({ queryConfig, query, page }: UseActorsOptions) => {
  return useQuery({
    ...getActorsQueryOptions({ query, page }),
    ...queryConfig
  })
}

type UseAllActorsOptions = {
  queryConfig?: QueryConfig<typeof getAllActorsQueryOptions>
}

export const useAllActors = ({ queryConfig }: UseAllActorsOptions) => {
  return useQuery({
    ...getAllActorsQueryOptions(),
    ...queryConfig
  })
}

export const getAllActorsQueryOptions = () => {
  return queryOptions({
    queryKey: ['actors'],
    queryFn: () => getAllActors()
  })
}
