import { useQuery, queryOptions } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { QueryConfig } from '@/lib/react-query'
import { Actor } from '@/types/api'

export const getActor = ({ uniqueId }: { uniqueId: string }): Promise<{ data: Actor }> => {
  const response = api.get(`/actors/${uniqueId}`)
  return response
}

export const getActorQueryOptions = (uniqueId: string) => {
  return queryOptions({
    queryKey: ['actors', uniqueId],
    queryFn: () => getActor({ uniqueId })
  })
}

type UseActorOptions = {
  uniqueId: string
  queryConfig?: QueryConfig<typeof getActorQueryOptions>
}

export const useActor = ({ uniqueId, queryConfig }: UseActorOptions) => {
  return useQuery({
    ...getActorQueryOptions(uniqueId),
    ...queryConfig
  })
}
