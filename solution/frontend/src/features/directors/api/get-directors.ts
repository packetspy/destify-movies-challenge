import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Director, Meta } from '@/types/api'

export const getDirectors = (
  query: string | undefined,
  page: number | undefined
): Promise<{
  data: Director[]
  meta: Meta
}> => {
  return api.get(`/directors/paginated`, {
    params: {
      query,
      page
    }
  })
}

export const getAllDirectors = () => {
  const response = api.get(`/directors`) as Promise<Director[]>
  return response
}

export const getDirectorsQueryOptions = ({
  query,
  page
}: { query?: string; page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['directors', { query, page }] : ['directors'],
    queryFn: () => getDirectors(query, page)
  })
}

type UseDirectorsOptions = {
  query?: string | undefined
  page?: number
  queryConfig?: QueryConfig<typeof getDirectorsQueryOptions>
}

export const useDirectors = ({ queryConfig, query, page }: UseDirectorsOptions) => {
  return useQuery({
    ...getDirectorsQueryOptions({ query, page }),
    ...queryConfig
  })
}

type UseAllDirectorsOptions = {
  queryConfig?: QueryConfig<typeof getAllDirectorsQueryOptions>
}

export const useAllDirectors = ({ queryConfig }: UseAllDirectorsOptions) => {
  return useQuery({
    ...getAllDirectorsQueryOptions(),
    ...queryConfig
  })
}

export const getAllDirectorsQueryOptions = () => {
  return queryOptions({
    queryKey: ['directors'],
    queryFn: () => getAllDirectors()
  })
}
