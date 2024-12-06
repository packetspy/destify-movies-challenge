import { useQuery, queryOptions } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Director } from '@/types/api'

export const getDirector = ({ uniqueId }: { uniqueId: string }): Promise<{ data: Director }> => {
  const response = api.get(`/directors/${uniqueId}`)
  return response
}

export const getDirectorQueryOptions = (uniqueId: string) => {
  return queryOptions({
    queryKey: ['directors', uniqueId],
    queryFn: () => getDirector({ uniqueId })
  })
}

type UseDirectorOptions = {
  uniqueId: string
  queryConfig?: QueryConfig<typeof getDirectorQueryOptions>
}

export const useDirector = ({ uniqueId, queryConfig }: UseDirectorOptions) => {
  return useQuery({
    ...getDirectorQueryOptions(uniqueId),
    ...queryConfig
  })
}
