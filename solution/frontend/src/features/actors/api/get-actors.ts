import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Actor, Meta } from '@/types/api'

export const getActors = (
  page = 1
): Promise<{
  data: Actor[]
  meta: Meta
}> => {
  return api.get(`/actors/paginated`, {
    params: {
      page
    }
  })
}

export const getAllActors = () => {
  const response = api.get(`/actors`) as Promise<Actor[]>
  return response
}

export const getActorsQueryOptions = ({ page }: { page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['actors', { page }] : ['actors'],
    queryFn: () => getActors(page)
  })
}

type UseActorsOptions = {
  page?: number
  queryConfig?: QueryConfig<typeof getActorsQueryOptions>
}

export const useActors = ({ queryConfig, page }: UseActorsOptions) => {
  return useQuery({
    ...getActorsQueryOptions({ page }),
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
