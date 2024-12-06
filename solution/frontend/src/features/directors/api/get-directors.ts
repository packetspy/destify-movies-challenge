import { queryOptions, useQuery } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Director, Meta } from '@/types/api'

export const getDirectors = (
  page = 1
): Promise<{
  data: Director[]
  meta: Meta
}> => {
  return api.get(`/directors/paginated`, {
    params: {
      page
    }
  })
}

export const getAllDirectors = () => {
  const response = api.get(`/directors`) as Promise<Director[]>
  return response
}

export const getDirectorsQueryOptions = ({ page }: { page?: number } = {}) => {
  return queryOptions({
    queryKey: page ? ['directors', { page }] : ['directors'],
    queryFn: () => getDirectors(page)
  })
}

type UseDirectorsOptions = {
  page?: number
  queryConfig?: QueryConfig<typeof getDirectorsQueryOptions>
}

export const useDirectors = ({ queryConfig, page }: UseDirectorsOptions) => {
  return useQuery({
    ...getDirectorsQueryOptions({ page }),
    ...queryConfig
  })
}

export const getAllDirectorsQueryOptions = () => {
  return queryOptions({
    queryKey: ['directors'],
    queryFn: () => getAllDirectors()
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
